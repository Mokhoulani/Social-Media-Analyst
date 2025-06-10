import { useCallback, useEffect, useState } from 'react'
import { AppState } from 'react-native'
import {
    hasUsagePermission,
    openPermissionSettings,
    startTracking,
    stopTracking,
} from '../helpers/AppUsageServiceBridge'

export const useAppUsageService = () => {
    const [hasPermission, setHasPermission] = useState<boolean>(false)
    const [isTracking, setIsTracking] = useState<boolean>(false)

    const checkPermission = useCallback(async () => {
        const granted = await hasUsagePermission()
        setHasPermission(granted)
        return granted
    }, [])

    const requestPermission = useCallback(() => {
        openPermissionSettings() // Opens settings screen
    }, [])

    const start = useCallback(async () => {
        const granted = await checkPermission()
        if (!granted) {
            requestPermission()
            return false
        }
        startTracking()
        setIsTracking(true)
        return true
    }, [checkPermission, requestPermission])

    const stop = useCallback(() => {
        stopTracking()
        setIsTracking(false)
    }, [])

    // Optional: handle app state changes
    useEffect(() => {
        checkPermission() // Check on mount

        const subscription = AppState.addEventListener(
            'change',
            async (state) => {
                if (state === 'active') {
                    checkPermission()
                }
            }
        )

        return () => subscription.remove()
    }, [checkPermission])

    return {
        hasPermission,
        isTracking,
        start,
        stop,
        requestPermission,
        checkPermission,
    }
}
