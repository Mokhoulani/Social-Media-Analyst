import React from 'react'
import { Image, StyleSheet, Text, TouchableOpacity } from 'react-native'
import { SocialMediaPlatform } from '../../entities/SocialMediaPlatform'

interface Props {
    platform: SocialMediaPlatform
    onSelect: (platform: SocialMediaPlatform) => void
}

export const PlatformCard: React.FC<Props> = ({ platform, onSelect }) => {
    return (
        <TouchableOpacity
            style={styles.card}
            onPress={() => onSelect(platform)}
        >
            <Image source={{ uri: platform.iconUrl }} style={styles.icon} />
            <Text style={styles.name}>{platform.name}</Text>
        </TouchableOpacity>
    )
}

const styles = StyleSheet.create({
    card: {
        flexDirection: 'row',
        alignItems: 'center',
        padding: 12,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 12,
        marginVertical: 6,
        backgroundColor: '#fff',
    },
    icon: {
        width: 32,
        height: 32,
        marginRight: 12,
    },
    name: {
        fontSize: 16,
        fontWeight: '500',
    },
})
