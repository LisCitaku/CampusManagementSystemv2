import React, { useState, useEffect } from 'react';
import {
  Container,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Box,
  Typography,
  CircularProgress,
  IconButton,
  Tooltip,
} from '@mui/material';
import { Edit, Delete, Add } from '@mui/icons-material';
import apiClient from '../api/client';
import { Classroom } from '../types';

export const ClassroomsManagementPage: React.FC = () => {
  const [classrooms, setClassrooms] = useState<Classroom[]>([]);
  const [loading, setLoading] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [selectedClassroom, setSelectedClassroom] = useState<Classroom | null>(null);
  const [formData, setFormData] = useState({
    type: '',
    capacity: 30,
  });

  useEffect(() => {
    fetchClassrooms();
  }, []);

  const fetchClassrooms = async () => {
    try {
      const res = await apiClient.getAllClassrooms();
      setClassrooms(res.data);
    } catch (error) {
      console.error('Failed to fetch classrooms:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenDialog = (classroom?: Classroom) => {
    if (classroom) {
      setSelectedClassroom(classroom);
      setFormData({
        type: classroom.type,
        capacity: classroom.capacity,
      });
    } else {
      setSelectedClassroom(null);
      setFormData({ type: '', capacity: 30 });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setSelectedClassroom(null);
  };

  const handleSave = async () => {
    try {
      if (selectedClassroom) {
        await apiClient.updateClassroom(selectedClassroom.classroomId, formData);
      } else {
        await apiClient.createClassroom(formData);
      }
      await fetchClassrooms();
      handleCloseDialog();
    } catch (error) {
      console.error('Failed to save classroom:', error);
    }
  };

  const handleDelete = async (classroomId: string) => {
    if (window.confirm('Are you sure?')) {
      try {
        await apiClient.deleteClassroom(classroomId);
        await fetchClassrooms();
      } catch (error) {
        console.error('Failed to delete classroom:', error);
      }
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
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 4 }}>
        <Typography variant="h4">Classrooms Management</Typography>
        <Button
          variant="contained"
          startIcon={<Add />}
          onClick={() => handleOpenDialog()}
        >
          Add Classroom
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead sx={{ backgroundColor: '#f5f5f5' }}>
            <TableRow>
              <TableCell>Type</TableCell>
              <TableCell>Capacity</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {classrooms.map((classroom) => (
              <TableRow key={classroom.classroomId}>
                <TableCell>{classroom.type}</TableCell>
                <TableCell>{classroom.capacity}</TableCell>
                <TableCell>
                  <Tooltip title="Edit">
                    <IconButton
                      size="small"
                      onClick={() => handleOpenDialog(classroom)}
                    >
                      <Edit />
                    </IconButton>
                  </Tooltip>
                  <Tooltip title="Delete">
                    <IconButton
                      size="small"
                      onClick={() => handleDelete(classroom.classroomId)}
                      color="error"
                    >
                      <Delete />
                    </IconButton>
                  </Tooltip>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
        <DialogTitle>{selectedClassroom ? 'Edit Classroom' : 'Add Classroom'}</DialogTitle>
        <DialogContent sx={{ pt: 2 }}>
          <TextField
            fullWidth
            label="Type"
            value={formData.type}
            onChange={(e) => setFormData({ ...formData, type: e.target.value })}
            margin="normal"
          />
          <TextField
            fullWidth
            label="Capacity"
            type="number"
            value={formData.capacity}
            onChange={(e) => setFormData({ ...formData, capacity: parseInt(e.target.value) })}
            margin="normal"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>Cancel</Button>
          <Button onClick={handleSave} variant="contained">
            Save
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};
