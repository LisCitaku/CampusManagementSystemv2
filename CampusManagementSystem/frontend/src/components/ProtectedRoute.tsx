import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../contexts/authStore';
import { useEffect } from 'react';

export const ProtectedRoute: React.FC<{ children: React.ReactNode; requiredRole?: string }> = ({
  children,
  requiredRole,
}) => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const user = useAuthStore((state) => state.user);
  const navigate = useNavigate();

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
    } else if (requiredRole && user?.roleType !== requiredRole) {
      navigate('/dashboard');
    }
  }, [isAuthenticated, user, requiredRole, navigate]);

  if (!isAuthenticated) {
    return null;
  }

  if (requiredRole && user?.roleType !== requiredRole) {
    return null;
  }

  return <>{children}</>;
};
