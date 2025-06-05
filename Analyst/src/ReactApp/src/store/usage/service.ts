import { Observable } from 'rxjs'
import { APIClient } from '../../api/apiClient'
import { UserSocialMediaUsage } from '../../entities/UserSocialMediaUsage'

export interface UsagePayload {
    userId: string
}

export const UsgaeService = {
    getUsage: (payload: UsagePayload): Observable<UserSocialMediaUsage[]> =>
        APIClient.post<UserSocialMediaUsage[]>(
            '/UserSocialMediaUsage/get-usages',
            payload
        ),

    setUsage: (usage: UserSocialMediaUsage): Observable<UserSocialMediaUsage> =>
        APIClient.post<UserSocialMediaUsage>(
            '/UserSocialMediaUsage/create-or-update-usage',
            usage
        ),
}
