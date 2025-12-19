import React, { useState, useEffect } from 'react';
import apiClient from '../api/client';
import { Course } from '../types';
import {
  Container,
  Grid,
  Card,
  CardContent,
  CardActions,
  Typography,
  Button,
  CircularProgress,
  Box,
  Alert,
} from '@mui/material';

export const CoursesPage: React.FC = () => {
  const [courses, setCourses] = useState<Course[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchCourses();
  }, []);

  const fetchCourses = async () => {
    try {
      const response = await apiClient.getAllCourses();
      setCourses(response.data);
    } catch (err: any) {
      setError('Failed to load courses');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleEnroll = async (courseId: string) => {
    try {
      await apiClient.enrollCourse('current-user-id', courseId);
      alert('Enrolled successfully!');
      fetchCourses();
    } catch (err: any) {
      alert('Failed to enroll');
    }
  };

  if (loading) {
    return <CircularProgress />;
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" gutterBottom>
        Available Courses
      </Typography>

      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

      <Grid container spacing={3}>
        {courses.map((course) => (
          <Grid item xs={12} sm={6} md={4} key={course.courseId}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  {course.title}
                </Typography>
                <Typography color="textSecondary">
                  Credits: {course.creditPoints}
                </Typography>
                <Typography color="textSecondary">
                  Enrolled: {course.currentEnrollments}/{course.maxCapacity}
                </Typography>
              </CardContent>
              <CardActions>
                <Button
                  size="small"
                  color="primary"
                  onClick={() => handleEnroll(course.courseId)}
                  disabled={course.currentEnrollments >= course.maxCapacity}
                >
                  Enroll
                </Button>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};
