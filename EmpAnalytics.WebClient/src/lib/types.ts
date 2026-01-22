export interface RecentlyActiveEmployeeResponse {
  firstName: string;
  lastName: string;
  jobName: string;
  lastJobDate: string;
}

export interface AboveAveragePerformerResponse {
  firstName: string;
  lastName: string;
  jobCount: number;
}

export interface AboveAverageFilters {
  startDate?: string;
  endDate?: string;
}

export interface EmployeeExceedingOwnAverageResponse {
  firstName: string;
  lastName: string;
  jobCountLast30Days: number;
}
