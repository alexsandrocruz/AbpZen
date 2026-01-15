import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface LoadingStateProps {
    message?: string;
    className?: string;
}

export function LoadingState({ message = "Loading...", className }: LoadingStateProps) {
    return (
        <div className={cn("flex min-h-[400px] flex-col items-center justify-center p-8", className)}>
            <Loader2 className="h-10 w-10 animate-spin text-primary" />
            <p className="mt-4 text-sm font-medium text-muted-foreground animate-pulse">
                {message}
            </p>
        </div>
    );
}

export function LoadingOverlay() {
    return (
        <div className="absolute inset-0 z-50 flex items-center justify-center bg-background/50 backdrop-blur-sm transition-all animate-in fade-in duration-300">
            <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
    );
}
