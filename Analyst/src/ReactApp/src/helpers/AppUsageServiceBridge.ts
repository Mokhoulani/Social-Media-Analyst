import { NativeModules, Platform } from 'react-native'

// Destructure the native module
const { AppUsageService } = NativeModules

/**
 * Starts the background usage tracking service.
 */
export const startTracking = () => {
    if (Platform.OS === 'android' && AppUsageService?.startService) {
        AppUsageService.startService()
    }
}

/**
 * Stops the background usage tracking service.
 */
export const stopTracking = () => {
    if (Platform.OS === 'android' && AppUsageService?.stopService) {
        AppUsageService.stopService()
    }
}

/**
 * Opens the settings screen for the user to grant Usage Access permission.
 */
export const openPermissionSettings = () => {
    if (Platform.OS === 'android' && AppUsageService?.openUsageAccessSettings) {
        AppUsageService.openUsageAccessSettings()
    }
}

/**
 * Returns whether the app has been granted Usage Access permission.
 * @returns Promise<boolean>
 */
export const hasUsagePermission = async (): Promise<boolean> => {
    if (Platform.OS === 'android' && AppUsageService?.hasUsagePermission) {
        try {
            return await AppUsageService.hasUsagePermission()
        } catch (e) {
            console.warn('Error checking usage permission:', e)
            return false
        }
    }
    return false
}
