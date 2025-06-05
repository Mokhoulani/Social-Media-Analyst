import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { firstValueFrom } from 'rxjs'
import { AuthActions } from '../store/auth/actions'
import { AuthFacade } from '../store/auth/facade'
import { UserFacade } from '../store/user/facade'
import { userSelectors } from '../store/user/selectors'
import {
    getStoredExpiration,
    getStoredRefreshToken,
    getStoredToken,
    isTokenExpired$,
} from '../utils/jwt-utils'

export const useAuthCheck = (): boolean => {
    const [checking, setChecking] = useState(true)
    const isAuthenticatedUser = useSelector(userSelectors.selectIsAuthenticated)

    useEffect(() => {
        const checkAuth = async () => {
            const accessToken = await getStoredToken()
            const refreshToken = await getStoredRefreshToken()
            const expiration = await getStoredExpiration()

            if (!accessToken || !refreshToken || !expiration) {
                setChecking(false)
                return
            }

            try {
                const isExpired = await firstValueFrom(isTokenExpired$())

                if (isExpired) {
                    await firstValueFrom(AuthFacade.refreshToken$())
                } else {
                    AuthActions.refreshTokenSuccess({
                        accessToken,
                        refreshToken,
                    })
                }

                await firstValueFrom(UserFacade.getUser())
            } catch (err) {
                console.error('Auth check failed:', err)
                // Optionally handle errors
            } finally {
                setChecking(false)
            }
        }

        checkAuth()
    }, [])

    return !checking && isAuthenticatedUser
}
