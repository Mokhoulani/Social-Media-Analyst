export const getStoredToken = (): string | null => {
    return localStorage.getItem('token')
}

export const getStoredRefreshToken = (): string | null => {
    return localStorage.getItem('refreshToken')
}

export const storeToken = (token: string, refreshToken: string): void => {
    localStorage.setItem('token', token)
    localStorage.setItem('refreshToken', refreshToken)
}

export const clearTokens = (): void => {
    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')
}
