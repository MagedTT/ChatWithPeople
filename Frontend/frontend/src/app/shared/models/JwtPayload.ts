export interface JwtPayload {
    sub: string;
    email: string;
    roles: string[];
    exp: number;
    iat: number;
}
