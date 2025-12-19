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
import { Facility } from '../types';

export const FacilitiesManagementPage: React.FC = () => {
  const [facilities, setFacilities] = useState<Facility[]>([]);
  const [loading, setLoading] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [selectedFacility, setSelectedFacility] = useState<Facility | null>(null);
  const [formData, setFormData] = useState({
    facilityType: '',
    status: 'Available',
  });

  useEffect(() => {
    fetchFacilities();
  }, []);

  const fetchFacilities = async () => {
    try {
      const res = await apiClient.getAllFacilities();
      setFacilities(res.data);
    } catch (error) {
      console.error('Failed to fetch facilities:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenDialog = (facility?: Facility) => {
    if (facility) {
      setSelectedFacility(facility);
      setFormData({
        facilityType: facility.facilityType,
        status: facility.status,
      });
    } else {
      setSelectedFacility(null);
      setFormData({ facilityType: '', status: 'Available' });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setSelectedFacility(null);
  };

  const handleSave = async () => {
    try {
      if (selectedFacility) {
        await apiClient.updateFacility(selectedFacility.facilityId, formData);
      } else {
        await apiClient.createFacility(formData);
      }
      await fetchFacilities();
      handleCloseDialog();
    } catch (error) {
      console.error('Failed to save facility:', error);
    }
  };

  const handleDelete = async (facilityId: string) => {
    if (window.confirm('Are you sure?')) {
      try {
        await apiClient.deleteFacility(facilityId);
        await fetchFacilities();
      } catch (error) {
        console.error('Failed to delete facility:', error);
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
        <Typography variant="h4">Facilities Management</Typography>
        <Button
          variant="contained"
          startIcon={<Add />}
          onClick={() => handleOpenDialog()}
        >
          Add Facility
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead sx={{ backgroundColor: '#f5f5f5' }}>
            <TableRow>
              <TableCell>Type</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {facilities.map((facility) => (
              <TableRow key={facility.facilityId}>
                <TableCell>{facility.facilityType}</TableCell>
                <TableCell>{facility.status}</TableCell>
                <TableCell>
                  <Tooltip title="Edit">
                    <IconButton
                      size="small"
                      onClick={() => handleOpenDialog(facility)}
                    >
                      <Edit />
                    </IconButton>
                  </Tooltip>
                  <Tooltip title="Delete">
                    <IconButton
                      size="small"
                      onClick={() => handleDelete(facility.facilityId)}
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
        <DialogTitle>{selectedFacility ? 'Edit Facility' : 'Add Facility'}</DialogTitle>
        <DialogContent sx={{ pt: 2 }}>
          <TextField
            fullWidth
            label="Facility Type"
            value={formData.facilityType}
            onChange={(e) => setFormData({ ...formData, facilityType: e.target.value })}
            margin="normal"
          />
          <TextField
            fullWidth
            label="Status"
            select
            SelectProps={{ native: true }}
            value={formData.status}
            onChange={(e) => setFormData({ ...formData, status: e.target.value })}
            margin="normal"
          >
            <option value="Available">Available</option>
            <option value="Maintenance">Maintenance</option>
            <option value="OutOfService">Out of Service</option>
          </TextField>
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
