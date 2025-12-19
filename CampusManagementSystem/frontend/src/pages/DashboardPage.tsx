import React, { useState, useEffect } from 'react';
import { useAuthStore } from '../contexts/authStore';
import apiClient from '../api/client';
import {
  Container,
  Grid,
  Paper,
  Typography,
  Card,
  CardContent,
  Box,
  CircularProgress,
} from '@mui/material';

export const DashboardPage: React.FC = () => {
  const user = useAuthStore((state) => state.user);
  const [stats, setStats] = useState({
    coursesCount: 0,
    enrollmentsCount: 0,
    facilitiesCount: 0,
    issuesCount: 0,
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchStats();
  }, []);

  const fetchStats = async () => {
    try {
      const [courses, facilities, issues] = await Promise.all([
        apiClient.getAllCourses(),
        apiClient.getAllFacilities(),
        apiClient.getOpenIssues().catch(() => ({ data: [] })),
      ]);

      setStats({
        coursesCount: courses.data.length,
        facilitiesCount: facilities.data.length,
        issuesCount: issues.data.length,
        enrollmentsCount: 0,
      });
    } catch (error) {
      console.error('Failed to fetch stats:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <Container sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <CircularProgress />
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" gutterBottom>
        Welcome, {user?.name}!
      </Typography>
      <Typography variant="body1" sx={{ mb: 4 }}>
        Role: {user?.roleType}
      </Typography>

      <Grid container spacing={3}>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Typography color="textSecondary" gutterBottom>
                Total Courses
              </Typography>
              <Typography variant="h5">{stats.coursesCount}</Typography>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Typography color="textSecondary" gutterBottom>
                Facilities
              </Typography>
              <Typography variant="h5">{stats.facilitiesCount}</Typography>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Typography color="textSecondary" gutterBottom>
                Open Issues
              </Typography>
              <Typography variant="h5">{stats.issuesCount}</Typography>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Typography color="textSecondary" gutterBottom>
                My Enrollments
              </Typography>
              <Typography variant="h5">{stats.enrollmentsCount}</Typography>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Container>
  );
};
