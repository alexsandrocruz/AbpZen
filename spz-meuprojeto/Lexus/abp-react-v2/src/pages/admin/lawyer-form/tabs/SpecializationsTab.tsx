import { Card } from "@/components/ui/card";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import { useAllSpecializations } from "@/lib/abp/hooks/useSpecializations";
import { Skeleton } from "@/components/ui/skeleton";

interface SpecializationsTabProps {
    lawyerId?: string;
    selectedSpecializations: Set<string>;
    onToggle: (specializationId: string, isChecked: boolean) => void;
}

export function SpecializationsTab({
    lawyerId,
    selectedSpecializations,
    onToggle
}: SpecializationsTabProps) {
    const { data: allSpecs, isLoading: loadingAll } = useAllSpecializations();

    if (loadingAll) {
        return (
            <Card className="p-6">
                <div className="space-y-3">
                    {[1, 2, 3, 4].map((i) => (
                        <div key={i} className="flex items-center space-x-2">
                            <Skeleton className="h-4 w-4" />
                            <Skeleton className="h-4 w-48" />
                        </div>
                    ))}
                </div>
            </Card>
        );
    }

    return (
        <Card className="p-6">
            <div className="space-y-3">
                {allSpecs?.items && allSpecs.items.length > 0 ? (
                    allSpecs.items.map((spec: any) => {
                        const isChecked = selectedSpecializations.has(spec.id!);

                        return (
                            <div key={spec.id} className="flex items-center space-x-2">
                                <Checkbox
                                    id={`spec-${spec.id}`}
                                    checked={isChecked}
                                    onCheckedChange={(checked) => onToggle(spec.id!, !!checked)}
                                />
                                <Label
                                    htmlFor={`spec-${spec.id}`}
                                    className="text-sm font-normal cursor-pointer"
                                >
                                    {spec.name}
                                </Label>
                            </div>
                        );
                    })
                ) : (
                    <p className="text-sm text-muted-foreground">
                        No specializations available. Please create some first.
                    </p>
                )}
            </div>
        </Card>
    );
}
