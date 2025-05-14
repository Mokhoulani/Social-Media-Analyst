import * as SplashScreen from 'expo-splash-screen';
import { useCallback, useEffect, useState } from 'react';


// Prevent the splash screen from auto-hiding
SplashScreen.preventAutoHideAsync();

export function useSplashScreen() {

    const [appIsReady, setAppIsReady] = useState(false);

    useEffect(() => {
        async function prepare() {
            try {
                // Initialize authentication

            } catch (e) {
                console.warn(e);
            } finally {
                // Tell the application to render
                setAppIsReady(true);
            }
        }

        prepare();
    }, []);

    const onLayoutRootView = useCallback(async () => {
        if (appIsReady) {
            // This tells the splash screen to hide immediately
            await SplashScreen.hideAsync();
        }
    }, [appIsReady]);

    return { appIsReady, onLayoutRootView };
}