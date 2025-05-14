import { NavigationContainer } from '@react-navigation/native';
import { StatusBar } from 'expo-status-bar';
import React from 'react';
import { useColorScheme } from 'react-native';
import { PaperProvider } from 'react-native-paper';
import RootNavigator from '../navigators/RootStackNavigator';
import { useAppSelector } from '../store/hook';
import { selectColorMode } from '../store/theme/selectors';
import { combinedDarkTheme, combinedLightTheme } from '../themes/theme';

export default function MainApp() {
    const colorMode = useAppSelector(selectColorMode); 
    const colorScheme = useColorScheme();

    const theme =
        colorMode === 'dark' || (colorMode === 'auto' && colorScheme === 'dark')
            ? combinedDarkTheme
            : combinedLightTheme;

    return (
        <>
            <StatusBar
                style={
                    colorMode === 'light'
                        ? 'light'
                        : colorMode === 'dark'
                            ? 'light'
                            : 'auto'
                }
            />
            <PaperProvider theme={theme}>
                <NavigationContainer theme={theme}>
                    <RootNavigator />
                </NavigationContainer>
            </PaperProvider>
        </>
    );
}
