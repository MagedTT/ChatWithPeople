import { UserStatus } from "./UserStatus";

export interface FriendMinimalInformation {
    id: string;
    userName: string;
    profilePicture?: string;
    status: UserStatus
}