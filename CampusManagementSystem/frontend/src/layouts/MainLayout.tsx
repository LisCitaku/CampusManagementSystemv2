import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../contexts/authStore';
import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Box,
  Container,
} from '@mui/material';

interface MainLayoutProps {
  children: React.ReactNode;
}

export const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  const user = useAuthStore((state) => state.user);
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const clearAuth = useAuthStore((state) => state.clearAuth);
  const navigate = useNavigate();

  const handleLogout = () => {
    clearAuth();
    navigate('/login');
  };

  if (!isAuthenticated) {
    return <>{children}</>;
  }

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Campus Management System
          </Typography>
          <Button color="inherit" href="/dashboard">
            Dashboard
          </Button>
          <Button color="inherit" href="/courses">
            Courses
          </Button>
          {user?.roleType === 'Admin' && (
            <>
              <Button color="inherit" href="/admin/students">
                Students
              </Button>
              <Button color="inherit" href="/admin/staff">
                Staff
              </Button>
              <Button color="inherit" href="/admin/classrooms">
                Classrooms
              </Button>
              <Button color="inherit" href="/admin/facilities">
                Facilities
              </Button>
              <Button color="inherit" href="/admin/issues">
                Issues
              </Button>
            </>
          )}
          <Button color="inherit" onClick={handleLogout}>
            Logout
          </Button>
        </Toolbar>
      </AppBar>
      <Container sx={{ py: 4, flex: 1 }}>{children}</Container>
    </Box>
  );
};
