import { Tag } from "./tag"

export interface GifItem {
    id: string
    name: string
    username: string
    path: string
    hash: string
    tags: Array<Tag>
}
