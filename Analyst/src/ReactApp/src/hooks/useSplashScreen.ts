import * as SplashScreen from 'expo-splash-screen'
import { useCallback, useEffect, useState } from 'react'
import { initializeAppAsync } from '../helpers/initializeAppAsync'

// Prevent the splash screen from auto-hiding
SplashScreen.preventAutoHideAsync()

export function useSplashScreen() {
    const [appIsReady, setAppIsReady] = useState(false)

    useEffect(() => {
        const prepare = async () => {
            await initializeAppAsync(setAppIsReady)
        }

        prepare()
    }, [])

    const onLayoutRootView = useCallback(async () => {
        if (appIsReady) {
            // This tells the splash screen to hide immediately
            await SplashScreen.hideAsync()
        }
    }, [appIsReady])

    return { appIsReady, onLayoutRootView }
}
