import { UserStatus } from "./UserStatus";

export interface FriendDto {
    friendId: string;
    userName: string;
    profilePicture?: string;
    userStatus: UserStatus;
}