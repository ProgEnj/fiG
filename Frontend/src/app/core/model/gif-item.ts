import { Tag } from "./tag"

export interface GifItem {
    id: number
    name: string
    username: string
    path: string
    hash: string
    tags: Array<Tag>
}
