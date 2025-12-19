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
  Chip,
} from '@mui/material';
import { Edit, Delete, Add } from '@mui/icons-material';
import apiClient from '../api/client';
import { IssueReport } from '../types';

export const IssueReportsPage: React.FC = () => {
  const [issues, setIssues] = useState<IssueReport[]>([]);
  const [loading, setLoading] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [selectedIssue, setSelectedIssue] = useState<IssueReport | null>(null);
  const [formData, setFormData] = useState({
    description: '',
    priority: 'Medium',
    status: 'Open',
  });

  useEffect(() => {
    fetchIssues();
  }, []);

  const fetchIssues = async () => {
    try {
      const res = await apiClient.getAllIssues();
      setIssues(res.data);
    } catch (error) {
      console.error('Failed to fetch issues:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenDialog = (issue?: IssueReport) => {
    if (issue) {
      setSelectedIssue(issue);
      setFormData({
        description: issue.description,
        priority: issue.priority,
        status: issue.status,
      });
    } else {
      setSelectedIssue(null);
      setFormData({ description: '', priority: 'Medium', status: 'Open' });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setSelectedIssue(null);
  };

  const handleSave = async () => {
    try {
      if (selectedIssue) {
        await apiClient.updateIssue(selectedIssue.issueId, formData);
      }
      await fetchIssues();
      handleCloseDialog();
    } catch (error) {
      console.error('Failed to save issue:', error);
    }
  };

  const handleDelete = async (issueId: string) => {
    if (window.confirm('Are you sure?')) {
      try {
        await apiClient.deleteIssue(issueId);
        await fetchIssues();
      } catch (error) {
        console.error('Failed to delete issue:', error);
      }
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Open':
        return 'error';
      case 'InProgress':
        return 'warning';
      case 'Resolved':
        return 'success';
      default:
        return 'default';
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
        <Typography variant="h4">Issue Reports</Typography>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead sx={{ backgroundColor: '#f5f5f5' }}>
            <TableRow>
              <TableCell>Description</TableCell>
              <TableCell>Priority</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Created By</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {issues.map((issue) => (
              <TableRow key={issue.issueId}>
                <TableCell>{issue.description}</TableCell>
                <TableCell>{issue.priority}</TableCell>
                <TableCell>
                  <Chip
                    label={issue.status}
                    color={getStatusColor(issue.status) as any}
                    size="small"
                  />
                </TableCell>
                <TableCell>{issue.createdBy.name || 'Unknown'}</TableCell>
                <TableCell>
                  <Tooltip title="Edit">
                    <IconButton
                      size="small"
                      onClick={() => handleOpenDialog(issue)}
                    >
                      <Edit />
                    </IconButton>
                  </Tooltip>
                  <Tooltip title="Delete">
                    <IconButton
                      size="small"
                      onClick={() => handleDelete(issue.issueId)}
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
        <DialogTitle>Edit Issue</DialogTitle>
        <DialogContent sx={{ pt: 2 }}>
          <TextField
            fullWidth
            label="Description"
            multiline
            rows={4}
            value={formData.description}
            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
            margin="normal"
          />
          <TextField
            fullWidth
            label="Priority"
            select
            SelectProps={{ native: true }}
            value={formData.priority}
            onChange={(e) => setFormData({ ...formData, priority: e.target.value })}
            margin="normal"
          >
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
            <option value="Critical">Critical</option>
          </TextField>
          <TextField
            fullWidth
            label="Status"
            select
            SelectProps={{ native: true }}
            value={formData.status}
            onChange={(e) => setFormData({ ...formData, status: e.target.value })}
            margin="normal"
          >
            <option value="Open">Open</option>
            <option value="InProgress">In Progress</option>
            <option value="Resolved">Resolved</option>
            <option value="Closed">Closed</option>
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
