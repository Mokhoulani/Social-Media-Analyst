import React from 'react'
import { FlatList, Text } from 'react-native'
import { SocialMediaPlatform } from '../../entities/SocialMediaPlatform'
import { PlatformCard } from './PlatformCard'

interface Props {
    platforms: SocialMediaPlatform[]
    onSelectPlatform: (platform: SocialMediaPlatform) => void
}

export const PlatformList: React.FC<Props> = ({
    platforms,
    onSelectPlatform,
}) => {
    return (
        <FlatList
            data={platforms}
            keyExtractor={(item) => item.id.toString()}
            renderItem={({ item }) => (
                <PlatformCard platform={item} onSelect={onSelectPlatform} />
            )}
            ListEmptyComponent={<Text>No platforms available</Text>}
            contentContainerStyle={{ padding: 16 }}
        />
    )
}
