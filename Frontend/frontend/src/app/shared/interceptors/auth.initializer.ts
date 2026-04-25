import { inject, Injectable } from "@angular/core";
import { TokenService } from "../services/token.service";
import { Router } from "@angular/router";

export function authInitializer() {
    const tokenService = inject(TokenService);
    const router = inject(Router);

    return async () => {
        const accessToken = tokenService.getAccessToken();

        // No token at all → nothing to restore
        if (!accessToken) return;

        // Token still valid → continue
        if (!tokenService.isTokenExpired(accessToken)) return;

        // Token expired → attempt silent refresh
        const refreshed = await tokenService.refreshAccessToken();

        if (!refreshed) {
            tokenService.clearTokens();
            router.navigate(['/login']);
        }
    };
}

@Injectable({
    providedIn: 'root'
})
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
            this.router.navigateByUrl('login');
        }

        return Promise.resolve();
    }
}
