export interface Room {
    roomId: number;
    roomNumber: string;
    status: Status;
    perNightCharges: number;
    roomType: RoomType;
  }
  
  export enum RoomType {
    SingleBed = 0,
    DoubleBed = 1,
    HoneyMoonSweet = 2
  }

  export enum Status{
    Available = 'Available',
    Reserved = 'Reserved',
  }
  