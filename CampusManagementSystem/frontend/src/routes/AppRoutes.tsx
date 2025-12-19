import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAuthStore } from '../contexts/authStore';
import { MainLayout } from '../layouts/MainLayout';
import { ProtectedRoute } from '../components/ProtectedRoute';
import { LoginPage } from '../pages/LoginPage';
import { DashboardPage } from '../pages/DashboardPage';
import { CoursesPage } from '../pages/CoursesPage';
import { StudentsManagementPage } from '../pages/StudentsManagementPage';
import { StaffManagementPage } from '../pages/StaffManagementPage';
import { ClassroomsManagementPage } from '../pages/ClassroomsManagementPage';
import { FacilitiesManagementPage } from '../pages/FacilitiesManagementPage';
import { IssueReportsPage } from '../pages/IssueReportsPage';

export const AppRoutes: React.FC = () => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        <Route
          path="/dashboard"
          element={
            <MainLayout>
              <ProtectedRoute>
                <DashboardPage />
              </ProtectedRoute>
            </MainLayout>
          }
        />

        <Route
          path="/courses"
          element={
            <MainLayout>
              <ProtectedRoute>
                <CoursesPage />
              </ProtectedRoute>
            </MainLayout>
          }
        />

        <Route
          path="/admin/students"
          element={
            <MainLayout>
              <ProtectedRoute requiredRole="Admin">
                <StudentsManagementPage />
              </ProtectedRoute>
            </MainLayout>
          }
        />

        <Route
          path="/admin/staff"
          element={
            <MainLayout>
              <ProtectedRoute requiredRole="Admin">
                <StaffManagementPage />
              </ProtectedRoute>
            </MainLayout>
          }
        />

        <Route
          path="/admin/classrooms"
          element={
            <MainLayout>
              <ProtectedRoute requiredRole="Admin">
                <ClassroomsManagementPage />
              </ProtectedRoute>
            </MainLayout>
          }
        />

        <Route
          path="/admin/facilities"
          element={
            <MainLayout>
              <ProtectedRoute requiredRole="Admin">
                <FacilitiesManagementPage />
              </ProtectedRoute>
            </MainLayout>
          }
        />

        <Route
          path="/admin/issues"
          element={
            <MainLayout>
              <ProtectedRoute requiredRole="Admin">
                <IssueReportsPage />
              </ProtectedRoute>
            </MainLayout>
          }
        />

        <Route
          path="/"
          element={isAuthenticated ? <Navigate to="/dashboard" /> : <Navigate to="/login" />}
        />
      </Routes>
    </BrowserRouter>
  );
};
