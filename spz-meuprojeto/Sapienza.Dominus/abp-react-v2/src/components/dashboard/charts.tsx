import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer,
    AreaChart,
    Area,
    BarChart,
    Bar
} from 'recharts';

const data = [
    { name: 'Mon', users: 400, requests: 2400 },
    { name: 'Tue', users: 300, requests: 1398 },
    { name: 'Wed', users: 200, requests: 9800 },
    { name: 'Thu', users: 278, requests: 3908 },
    { name: 'Fri', users: 189, requests: 4800 },
    { name: 'Sat', users: 239, requests: 3800 },
    { name: 'Sun', users: 349, requests: 4300 },
];

export function OverviewChart() {
    return (
        <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
                <AreaChart data={data}>
                    <defs>
                        <linearGradient id="colorUsers" x1="0" y1="0" x2="0" y2="1">
                            <stop offset="5%" stopColor="var(--color-primary, #3b82f6)" stopOpacity={0.1} />
                            <stop offset="95%" stopColor="var(--color-primary, #3b82f6)" stopOpacity={0} />
                        </linearGradient>
                    </defs>
                    <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="hsl(var(--border))" />
                    <XAxis
                        dataKey="name"
                        stroke="hsl(var(--muted-foreground))"
                        fontSize={12}
                        tickLine={false}
                        axisLine={false}
                    />
                    <YAxis
                        stroke="hsl(var(--muted-foreground))"
                        fontSize={12}
                        tickLine={false}
                        axisLine={false}
                        tickFormatter={(value) => `${value}`}
                    />
                    <Tooltip
                        contentStyle={{
                            backgroundColor: 'hsl(var(--card))',
                            border: '1px solid hsl(var(--border))',
                            borderRadius: 'var(--radius-md)'
                        }}
                    />
                    <Area
                        type="monotone"
                        dataKey="users"
                        stroke="hsl(var(--primary))"
                        fillOpacity={1}
                        fill="url(#colorUsers)"
                        strokeWidth={2}
                    />
                </AreaChart>
            </ResponsiveContainer>
        </div>
    );
}

export function RequestsChart() {
    return (
        <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
                <BarChart data={data}>
                    <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="hsl(var(--border))" />
                    <XAxis
                        dataKey="name"
                        stroke="hsl(var(--muted-foreground))"
                        fontSize={12}
                        tickLine={false}
                        axisLine={false}
                    />
                    <YAxis
                        stroke="hsl(var(--muted-foreground))"
                        fontSize={12}
                        tickLine={false}
                        axisLine={false}
                    />
                    <Tooltip
                        contentStyle={{
                            backgroundColor: 'hsl(var(--card))',
                            border: '1px solid hsl(var(--border))',
                            borderRadius: 'var(--radius-md)'
                        }}
                    />
                    <Bar
                        dataKey="requests"
                        fill="hsl(var(--primary))"
                        radius={[4, 4, 0, 0]}
                        opacity={0.8}
                    />
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
}
