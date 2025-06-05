import { StyleSheet } from 'react-native'
import { useTheme } from 'react-native-paper'

export const useStyles = () => {
    const { colors } = useTheme()

    return StyleSheet.create({
        modal: {
            justifyContent: 'center',
            margin: 50,
        },
        modalContainer: {
            flex: 1,
            backgroundColor: colors.background,
        },
        container: {
            flex: 1,
            padding: 20,
        },
        dropdown: {
            height: 50,
            borderColor: colors.outline,
            borderWidth: 1,
            borderRadius: 5,
            paddingHorizontal: 12,
            marginBottom: 10,
            justifyContent: 'center',
            color: colors.onSurface,
            backgroundColor: colors.surface,
        },
        placeholderStyle: {
            fontSize: 16,
            color: colors.onSurfaceDisabled,
        },
        selectedTextStyle: {
            fontSize: 16,
            color: colors.onSurface,
            backgroundColor: colors.surface,
        },
        header: {
            flexDirection: 'row',
            justifyContent: 'space-between',
            alignItems: 'center',
            marginBottom: 20,
            paddingBottom: 10,
            borderBottomWidth: 1,
            borderBottomColor: colors.outlineVariant,
        },
        title: {
            fontSize: 24,
            fontWeight: 'bold',
            color: colors.onSurface,
        },
        closeButton: {
            width: 30,
            height: 30,
            borderRadius: 15,
            backgroundColor: colors.surfaceVariant,
            justifyContent: 'center',
            alignItems: 'center',
        },
        closeButtonText: {
            fontSize: 20,
            fontWeight: 'bold',
            color: colors.onSurfaceVariant,
        },
        content: {
            flex: 1,
        },
        label: {
            fontSize: 16,
            fontWeight: '500',
            marginTop: 15,
            marginBottom: 5,
            color: colors.onSurface,
        },
        input: {
            borderWidth: 1,
            borderColor: colors.outline,
            borderRadius: 5,
            padding: 10,
            marginBottom: 10,
            fontSize: 16,
            color: colors.onSurface,
            backgroundColor: colors.surface,
        },
        error: {
            color: colors.error,
            fontSize: 14,
            marginBottom: 10,
        },
        buttonContainer: {
            flexDirection: 'row',
            justifyContent: 'space-evenly',
            paddingTop: 20,
        },
        primaryButton: {
            backgroundColor: colors.primary,
            paddingVertical: 12,
            paddingHorizontal: 20,
            borderRadius: 8,
            alignItems: 'center',
        },
        primaryButtonText: {
            color: colors.onPrimary,
            fontSize: 16,
            fontWeight: '600',
        },
        secondaryButton: {
            backgroundColor: 'transparent',
            paddingVertical: 12,
            paddingHorizontal: 20,
            borderRadius: 8,
            alignItems: 'center',
            borderWidth: 1,
            borderColor: colors.outline,
        },
        secondaryButtonText: {
            color: colors.onSurfaceVariant,
            fontSize: 16,
        },
        disabledButton: {
            opacity: 0.5,
        },
        loading: {
            marginVertical: 20,
        },
        surface: {
            marginVertical: 6,
            borderRadius: 8,
            elevation: 2,
        },
        updateText: {
            marginRight: 16,
            color: colors.onSurface,
            fontWeight: 'bold',
        },
        emptyListContainer: {
            flexGrow: 1,
            justifyContent: 'center',
            alignItems: 'center',
        },
        emptyText: {
            marginTop: 20,
            fontSize: 16,
            color: colors.onSecondary,
        },
        success: {
            marginTop: 12,
            color: colors.onSurface,
            textAlign: 'center',
        },
        scrollContent: {
            paddingBottom: 20,
        },
        card: {
            backgroundColor: colors.surface,
            padding: 16,
            borderRadius: 12,
            marginBottom: 12,
            shadowColor: colors.shadow,
            shadowOffset: { width: 0, height: 2 },
            shadowOpacity: 0.1,
            shadowRadius: 4,
            elevation: 3,
        },
        platformName: {
            fontSize: 18,
            fontWeight: '600',
            marginBottom: 4,
        },
        usageText: {
            fontSize: 16,
            marginBottom: 2,
        },
        limitText: {
            fontSize: 14,
            color: '#666',
            marginBottom: 8,
        },
        progressBar: {
            height: 10,
            borderRadius: 5,
            backgroundColor: colors.surface,
        },
        warningText: {
            color: colors.error,
            fontSize: 16,
            fontWeight: 'bold',
            marginTop: 6,
        },

        row: {
            flexDirection: 'row',
            justifyContent: 'space-between',
            marginBottom: 6,
        },
        value: {
            fontWeight: '400',
        },
        warning: {
            color: colors.error,
            fontSize: 16,
            fontWeight: '600',
            marginTop: 8,
        },
    })
}

// Export theme colors separately if needed
export const useThemeColors = () => {
    const { colors } = useTheme()
    return colors
}
