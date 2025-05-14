import React from 'react';
import { ActivityIndicator, StyleSheet, View } from 'react-native';
import { useSplashScreen } from '../hooks/useSplashScreen';

export default function SplashScreen() {
    const { onLayoutRootView } = useSplashScreen();
    return (
        <View style= { styles.container } onLayout = { onLayoutRootView } >
            <ActivityIndicator size="large" color = "#0000ff" />
                </View>
  );
}

const styles = StyleSheet.create({
    container: { flex: 1 },
});