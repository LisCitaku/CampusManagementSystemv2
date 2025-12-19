import axios, { AxiosInstance, AxiosError } from 'axios';

const API_BASE_URL = 'http://localhost:5000/api';

class ApiClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Request interceptor to add token
    this.client.interceptors.request.use((config) => {
      const token = localStorage.getItem('token');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });

    // Response interceptor to handle errors
    this.client.interceptors.response.use(
      (response) => response,
      (error: AxiosError) => {
        if (error.response?.status === 401) {
          localStorage.removeItem('token');
          localStorage.removeItem('user');
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  // ===== AUTH ENDPOINTS =====
  async login(email: string, password: string) {
    return this.client.post('/auth/login', { email, password });
  }

  async register(name: string, email: string, password: string, roleType: string) {
    return this.client.post('/auth/register', { name, email, password, roleType });
  }

  async logout() {
    return this.client.post('/auth/logout', {});
  }

  async refreshToken() {
    return this.client.post('/auth/refresh-token', {});
  }

  // ===== USER ENDPOINTS =====
  async getUser(userId: string) {
    return this.client.get(`/users/${userId}`);
  }

  async getAllUsers() {
    return this.client.get('/users');
  }

  async createUser(data: any) {
    return this.client.post('/users', data);
  }

  async updateUser(userId: string, data: any) {
    return this.client.put(`/users/${userId}`, data);
  }

  async deleteUser(userId: string) {
    return this.client.delete(`/users/${userId}`);
  }

  // ===== STUDENT ENDPOINTS =====
  async getStudent(studentId: string) {
    return this.client.get(`/students/${studentId}`);
  }

  async getAllStudents() {
    return this.client.get('/students');
  }

  async createStudent(data: any) {
    return this.client.post('/students', data);
  }

  async updateStudent(studentId: string, data: any) {
    return this.client.put(`/students/${studentId}`, data);
  }

  async getStudentEnrollments(studentId: string) {
    return this.client.get(`/students/${studentId}/enrollments`);
  }

  // ===== STAFF ENDPOINTS =====
  async getStaff(staffId: string) {
    return this.client.get(`/staff/${staffId}`);
  }

  async getAllStaff() {
    return this.client.get('/staff');
  }

  async createStaff(data: any) {
    return this.client.post('/staff', data);
  }

  async updateStaff(staffId: string, data: any) {
    return this.client.put(`/staff/${staffId}`, data);
  }

  async deleteStaff(staffId: string) {
    return this.client.delete(`/staff/${staffId}`);
  }

  // ===== COURSE ENDPOINTS =====
  async getCourse(courseId: string) {
    return this.client.get(`/courses/${courseId}`);
  }

  async getAllCourses() {
    return this.client.get('/courses');
  }

  async createCourse(data: any) {
    return this.client.post('/courses', data);
  }

  async updateCourse(courseId: string, data: any) {
    return this.client.put(`/courses/${courseId}`, data);
  }

  async deleteCourse(courseId: string) {
    return this.client.delete(`/courses/${courseId}`);
  }

  async getCourseEnrollments(courseId: string) {
    return this.client.get(`/courses/${courseId}/enrollments`);
  }

  // ===== ENROLLMENT ENDPOINTS =====
  async getEnrollment(enrollmentId: string) {
    return this.client.get(`/enrollments/${enrollmentId}`);
  }

  async getAllEnrollments() {
    return this.client.get('/enrollments');
  }

  async getStudentEnrollmentsList(studentId: string) {
    return this.client.get(`/enrollments/student/${studentId}`);
  }

  async enrollCourse(studentId: string, courseId: string) {
    return this.client.post('/enrollments', { studentId, courseId });
  }

  async updateEnrollment(enrollmentId: string, data: any) {
    return this.client.put(`/enrollments/${enrollmentId}`, data);
  }

  async deleteEnrollment(enrollmentId: string) {
    return this.client.delete(`/enrollments/${enrollmentId}`);
  }

  // ===== CLASSROOM ENDPOINTS =====
  async getClassroom(classroomId: string) {
    return this.client.get(`/classrooms/${classroomId}`);
  }

  async getAllClassrooms() {
    return this.client.get('/classrooms');
  }

  async createClassroom(data: any) {
    return this.client.post('/classrooms', data);
  }

  async updateClassroom(classroomId: string, data: any) {
    return this.client.put(`/classrooms/${classroomId}`, data);
  }

  async deleteClassroom(classroomId: string) {
    return this.client.delete(`/classrooms/${classroomId}`);
  }

  // ===== RESERVATION ENDPOINTS =====
  async getReservation(reservationId: string) {
    return this.client.get(`/reservations/${reservationId}`);
  }

  async getAllReservations() {
    return this.client.get('/reservations');
  }

  async getStaffReservations(staffId: string) {
    return this.client.get(`/reservations/staff/${staffId}`);
  }

  async getClassroomReservations(classroomId: string) {
    return this.client.get(`/reservations/classroom/${classroomId}`);
  }

  async createReservation(data: any) {
    return this.client.post('/reservations', data);
  }

  async updateReservation(reservationId: string, data: any) {
    return this.client.put(`/reservations/${reservationId}`, data);
  }

  async deleteReservation(reservationId: string) {
    return this.client.delete(`/reservations/${reservationId}`);
  }

  async checkReservationAvailability(classroomId: string, startTime: string, endTime: string) {
    return this.client.get('/reservations/check-availability', {
      params: { classroomId, startTime, endTime },
    });
  }

  // ===== FACILITY ENDPOINTS =====
  async getFacility(facilityId: string) {
    return this.client.get(`/facilities/${facilityId}`);
  }

  async getAllFacilities() {
    return this.client.get('/facilities');
  }

  async createFacility(data: any) {
    return this.client.post('/facilities', data);
  }

  async updateFacility(facilityId: string, data: any) {
    return this.client.put(`/facilities/${facilityId}`, data);
  }

  async deleteFacility(facilityId: string) {
    return this.client.delete(`/facilities/${facilityId}`);
  }

  // ===== ISSUE REPORT ENDPOINTS =====
  async getIssue(issueId: string) {
    return this.client.get(`/issuereports/${issueId}`);
  }

  async getAllIssues() {
    return this.client.get('/issuereports');
  }

  async getOpenIssues() {
    return this.client.get('/issuereports/open');
  }

  async getFacilityIssues(facilityId: string) {
    return this.client.get(`/issuereports/facility/${facilityId}`);
  }

  async createIssue(data: any) {
    return this.client.post('/issuereports', data);
  }

  async updateIssue(issueId: string, data: any) {
    return this.client.put(`/issuereports/${issueId}`, data);
  }

  async deleteIssue(issueId: string) {
    return this.client.delete(`/issuereports/${issueId}`);
  }

  async assignIssue(issueId: string, staffId: string) {
    return this.client.post(`/issuereports/${issueId}/assign`, { staffId });
  }

  async resolveIssue(issueId: string) {
    return this.client.post(`/issuereports/${issueId}/resolve`, {});
  }
}

export default new ApiClient();
