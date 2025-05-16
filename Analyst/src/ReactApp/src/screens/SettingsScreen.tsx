import { ScrollView, StyleSheet } from 'react-native'
import { SegmentedButtons } from 'react-native-paper'
import { useAppDispatch, useAppSelector } from '../store/hook'
import { ThemeActions } from '../store/theme/actions'
import { selectColorMode } from '../store/theme/selectors'

export default function SettingsScreen() {
    const colorMode = useAppSelector(selectColorMode)

    const dispatch = useAppDispatch()

    const handleValueChange = (value: 'light' | 'dark' | 'auto') => {
        dispatch(ThemeActions.setColorMode({ colorMode: value }))
    }

    return (
        <ScrollView contentContainerStyle={styles.container}>
            <SegmentedButtons
                value={colorMode}
                onValueChange={handleValueChange}
                buttons={[
                    { value: 'light', label: 'Light' },
                    { value: 'dark', label: 'Dark' },
                    { value: 'auto', label: 'Auto' },
                ]}
            />
        </ScrollView>
    )
}

const styles = StyleSheet.create({
    container: {
        padding: 12,
        gap: 12,
    },
})
