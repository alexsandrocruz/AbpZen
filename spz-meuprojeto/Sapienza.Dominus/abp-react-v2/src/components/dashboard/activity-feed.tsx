import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";

const activities = [
    {
        id: "1",
        user: "Alex Cruz",
        action: "created a new workspace",
        target: "ZenCode V2",
        time: "2 hours ago",
        initials: "AC"
    },
    {
        id: "2",
        user: "System",
        action: "updated configuration for",
        target: "tenant-bravo",
        time: "5 hours ago",
        initials: "S"
    },
    {
        id: "3",
        user: "Mariana Silva",
        action: "invited 3 new users to",
        target: "Marketing Team",
        time: "1 day ago",
        initials: "MS"
    },
    {
        id: "4",
        user: "Lucas Rocha",
        action: "changed role permissions for",
        target: "Manager",
        time: "2 days ago",
        initials: "LR"
    }
];

export function ActivityFeed() {
    return (
        <div className="space-y-8">
            {activities.map((activity) => (
                <div key={activity.id} className="flex items-center">
                    <Avatar className="h-9 w-9">
                        <AvatarImage src={`https://avatar.vercel.sh/${activity.user}.png`} alt={activity.user} />
                        <AvatarFallback>{activity.initials}</AvatarFallback>
                    </Avatar>
                    <div className="ml-4 space-y-1">
                        <p className="text-sm font-medium leading-none">
                            <span className="font-bold">{activity.user}</span> {activity.action}{" "}
                            <span className="text-primary font-medium">{activity.target}</span>
                        </p>
                        <p className="text-xs text-muted-foreground">
                            {activity.time}
                        </p>
                    </div>
                </div>
            ))}
        </div>
    );
}
