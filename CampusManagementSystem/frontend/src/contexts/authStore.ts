import { create } from 'zustand';
import { LoginResponse } from '../types';

interface AuthStore {
  user: LoginResponse | null;
  token: string | null;
  isAuthenticated: boolean;
  isInitialized: boolean;
  setAuth: (user: LoginResponse, token: string) => void;
  clearAuth: () => void;
  loadFromStorage: () => void;
}

export const useAuthStore = create<AuthStore>((set) => {
  // Load initial state from localStorage
  const initialUser = localStorage.getItem('user');
  const initialToken = localStorage.getItem('token');
  
  return {
    user: initialUser ? JSON.parse(initialUser) : null,
    token: initialToken || null,
    isAuthenticated: !!(initialUser && initialToken),
    isInitialized: true,
    
    setAuth: (user, token) => {
      localStorage.setItem('user', JSON.stringify(user));
      localStorage.setItem('token', token);
      set({ user, token, isAuthenticated: true });
    },
    
    clearAuth: () => {
      localStorage.removeItem('user');
      localStorage.removeItem('token');
      set({ user: null, token: null, isAuthenticated: false });
    },
    
    loadFromStorage: () => {
      const user = localStorage.getItem('user');
      const token = localStorage.getItem('token');
      if (user && token) {
        set({ user: JSON.parse(user), token, isAuthenticated: true });
      }
    }
  };
});
