import { UserStatus } from "./UserStatus";

export interface FriendWithLastSentMessage {
    friendId: string;
    userName: string;
    profilePicture?: string;
    lastActive: Date;
    userStatus: UserStatus;
    lastSentMessage?: string;
}