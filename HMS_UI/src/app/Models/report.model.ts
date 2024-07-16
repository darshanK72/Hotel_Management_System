export interface Report {
    reportId: number;
    reportType: string;
    generatedDate: Date;
    reservationReport?: ReservationReport;
    staffReport?: StaffReport;
  }
  
  export interface ReservationReport {
    month: string;
    totalNumberOfReservations: number;
    totalIncome: number;
    roomReservations: RoomReservation[];
  }
  
  export interface RoomReservation {
    roomId: number;
    roomType: RoomType;
    roomNumber: string;
    numberOfTimeReserved: number;
    totalIncomeFromRoom: number;
  }
  
  export enum RoomType {
    SingleBed,
    DoubleBed,
    HoneyMoonSweet
  }
  
  export interface StaffReport {
    month: string;
    totalNumberOfStaff: number;
    totalSalary: number;
    staffSalaries: StaffSalary[];
  }
  
  export interface StaffSalary {
    staffId: number;
    staffName: string;
    salary: number;
    department: string;
  }
  