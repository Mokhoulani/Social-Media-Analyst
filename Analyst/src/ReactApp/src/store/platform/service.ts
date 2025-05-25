import { Observable } from 'rxjs'
import { APIClient } from '../../api/apiClient'
import { SocialMediaPlatform } from '../../entities/SocialMediaPlatform'

export interface PlatformError {
    message: string
}

export const PlatformService = {
    getPlatforms: (): Observable<SocialMediaPlatform[]> =>
        APIClient.get<SocialMediaPlatform[]>('/SocialMediaPlatform/get-all'),
}
