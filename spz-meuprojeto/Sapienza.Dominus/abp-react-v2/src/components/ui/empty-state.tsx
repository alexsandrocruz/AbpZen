import { Ghost, type LucideIcon } from "lucide-react";
import { Button } from "@/components/ui/button";

interface EmptyStateProps {
    title: string;
    description: string;
    icon?: LucideIcon;
    actionLabel?: string;
    onAction?: () => void;
}

export function EmptyState({
    title,
    description,
    icon: Icon = Ghost,
    actionLabel,
    onAction,
}: EmptyStateProps) {
    return (
        <div className="flex min-h-[400px] flex-col items-center justify-center rounded-lg border border-dashed p-8 text-center animate-in fade-in duration-500">
            <div className="flex h-20 w-20 items-center justify-center rounded-full bg-muted">
                <Icon className="h-10 w-10 text-muted-foreground" />
            </div>
            <h3 className="mt-4 text-lg font-semibold">{title}</h3>
            <p className="mt-2 mb-6 max-w-sm text-sm text-muted-foreground">
                {description}
            </p>
            {actionLabel && onAction && (
                <Button onClick={onAction} className="gap-2">
                    {actionLabel}
                </Button>
            )}
        </div>
    );
}
