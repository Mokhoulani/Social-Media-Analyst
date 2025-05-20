import * as SecureStore from 'expo-secure-store'
import { useEffect } from 'react'
import APIClient from '../api/apiClient'
import { registerForPushNotificationsAsync } from '../notifications/notificationService'

const DEVICE_TOKEN_KEY = 'device_token'
const DEVICE_ID_KEY = 'unique_device_id'

export const useRegisterPushToken = (isAuthenticated: boolean) => {
    useEffect(() => {
        if (!isAuthenticated) return

        let isMounted = true

        const syncToken = async () => {
            try {
                const newToken = await registerForPushNotificationsAsync()
                if (!newToken || !isMounted) return

                const storedToken =
                    await SecureStore.getItemAsync(DEVICE_TOKEN_KEY)
                const deviceId = await SecureStore.getItemAsync(DEVICE_ID_KEY)

                if (storedToken !== newToken) {
                    APIClient.post('/user/device', {
                        deviceToken: newToken,
                        deviceId: deviceId,
                    })
                    await SecureStore.setItemAsync(DEVICE_TOKEN_KEY, newToken)
                }
            } catch (error) {
                console.error('Error registering device token:', error)
            }
        }

        syncToken()

        return () => {
            isMounted = false
        }
    }, [isAuthenticated])
}
