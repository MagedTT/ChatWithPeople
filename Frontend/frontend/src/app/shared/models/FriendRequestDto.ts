import { FriendRequestStatus } from "./FriendRequestStatus";

export interface FriendRequestDto {
    id: string;
    userId: string;
    userName: string;
    profilePicture?: string;
    friendRequestStatus: FriendRequestStatus;
    createdAt: Date;
}

