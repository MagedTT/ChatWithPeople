import { UserStatus } from "./UserStatus";

export interface UserDto {
    id: string;
    userName: string;
    status: UserStatus,
    profilePicture: string | null;
    lastSeen: Date;
    CreatedAt: Date;
    isDeleted: boolean;
    DeletedAt: Date | null;
    age: number;
}