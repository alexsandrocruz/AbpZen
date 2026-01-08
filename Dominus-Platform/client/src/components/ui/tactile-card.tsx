import { cn } from "@/lib/utils";
import React from "react";

interface CardProps extends React.HTMLAttributes<HTMLDivElement> {
  hover?: boolean;
}

export const TactileCard = React.forwardRef<HTMLDivElement, CardProps>(
  ({ className, hover = false, ...props }, ref) => {
    return (
      <div
        ref={ref}
        className={cn(
          "rounded-xl border bg-card text-card-foreground shadow-sm transition-all duration-200",
          hover && "hover:shadow-md hover:border-primary/20 hover:-translate-y-0.5 cursor-pointer",
          className
        )}
        {...props}
      />
    );
  }
);
TactileCard.displayName = "TactileCard";
