import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { UserFacade } from '../store/user/facade'
import { userSelectors } from '../store/user/selectors'
import { getStoredRefreshToken, getStoredToken } from '../utils/jwt-utils'

export const useAuthCheck = (): boolean => {
    const [checking, setChecking] = useState(true)
    const isAuthenticatedUser = useSelector(userSelectors.selectIsAuthenticated)

    useEffect(() => {
        const checkAuth = async () => {
            const token = await getStoredToken()
            const refreshToken = await getStoredRefreshToken()

            if (!token || !refreshToken) {
                setChecking(false)
                return
            }

            try {
                await UserFacade.getUser()
            } catch {
                // Intentionally left blank: handle error silently
            } finally {
                setChecking(false)
            }
        }

        checkAuth()
    }, [])

    return !checking && isAuthenticatedUser
}
