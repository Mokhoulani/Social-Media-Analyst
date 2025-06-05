import { firstValueFrom } from 'rxjs'
import { getDeviceId } from './getDeviceId'
import { registerPushToken } from './registerPushToken'
import { AuthActions } from '../store/auth/actions'
import { AuthFacade } from '../store/auth/facade'
import { store } from '../store/store'
import { UserFacade } from '../store/user/facade'
import {
    getStoredRefreshToken,
    getStoredToken,
    isTokenExpired$,
} from '../utils/jwt-utils'

export async function initializeAppAsync(
    setAppIsReady: (ready: boolean) => void
): Promise<void> {
    try {
        const accessToken = await getStoredToken()
        const refreshToken = await getStoredRefreshToken()

        if (!accessToken || !refreshToken) {
            setAppIsReady(true)
            return
        }

        const expired = await firstValueFrom(isTokenExpired$())

        if (expired) {
            await firstValueFrom(AuthFacade.refreshToken$())
        } else {
            store.dispatch(
                AuthActions.refreshTokenSuccess({
                    accessToken,
                    refreshToken,
                })
            )
        }

        await firstValueFrom(UserFacade.getUser())

        const deviceId = await getDeviceId()
        await registerPushToken(true, deviceId)
    } catch (e) {
        console.warn('Error during app initialization:', e)
    } finally {
        setAppIsReady(true)
    }
}
