// Auth Types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  userId: string;
  name: string;
  email: string;
  roleType: string;
  token: string;
}

export interface User {
  userId: string;
  name: string;
  email: string;
  roleType: string;
  status: string;
  createdAt: string;
}

// Student Types
export interface Student extends User {
  studentNumber: string;
  yearOfStudy: number;
}

// Course Types
export interface Course {
  courseId: string;
  title: string;
  creditPoints: number;
  maxCapacity: number;
  currentEnrollments: number;
}

// Enrollment Types
export interface Enrollment {
  enrollmentId: string;
  studentId: string;
  courseId: string;
  studentName: string;
  courseTitle: string;
  status: string;
  timestamp: string;
}

// Classroom Types
export interface Classroom {
  classroomId: string;
  type: string;
  capacity: number;
}

// Reservation Types
export interface Reservation {
  reservationId: string;
  classroomId: string;
  staffId: string;
  classroomType: string;
  staffName: string;
  startTime: string;
  endTime: string;
  status: string;
}

// Facility Types
export interface Facility {
  facilityId: string;
  facilityType: string;
  status: string;
}

// Issue Report Types
export interface IssueReport {
  createdBy: any;
  issueId: string;
  facilityId: string;
  createdById: string;
  assignedToId: string | null;
  facilityType: string;
  createdByName: string;
  assignedToName: string | null;
  description: string;
  priority: string;
  status: string;
  createdAt: string;
}
