import { Router } from "@angular/router";
import { TokenService } from "../services/token.service";

export class AppInitializer {
    constructor(private tokenService: TokenService, private router: Router) { }

    async init(): Promise<void> {
        const accessToken = this.tokenService.getAccessToken();

        if (!accessToken)
            return Promise.resolve();

        if (!this.tokenService.isTokenExpired(accessToken))
            return Promise.resolve();

        const refreshed = await this.tokenService.refreshAccessToken();

        if (!refreshed) {
            this.tokenService.clearTokens();
            this.router.navigateByUrl('auth/login');
        }
    }
}