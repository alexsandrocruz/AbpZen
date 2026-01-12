import { Card } from "@/components/ui/card";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";

interface GeneralTabProps {
    formData: {
        fullName: string;
        preferredName: string;
    };
    onChange: (field: string, value: string) => void;
    errors?: Record<string, string>;
}

export function GeneralTab({ formData, onChange, errors }: GeneralTabProps) {
    return (
        <Card className="p-6">
            <div className="space-y-4">
                <div className="space-y-2">
                    <Label htmlFor="fullName">
                        Full Name <span className="text-red-500">*</span>
                    </Label>
                    <Input
                        id="fullName"
                        value={formData.fullName}
                        onChange={(e) => onChange('fullName', e.target.value)}
                        placeholder="Enter full name"
                        className={errors?.fullName ? 'border-red-500' : ''}
                    />
                    {errors?.fullName && (
                        <p className="text-sm text-red-500">{errors.fullName}</p>
                    )}
                </div>

                <div className="space-y-2">
                    <Label htmlFor="preferredName">Preferred Name</Label>
                    <Input
                        id="preferredName"
                        value={formData.preferredName}
                        onChange={(e) => onChange('preferredName', e.target.value)}
                        placeholder="Enter preferred name (optional)"
                    />
                </div>
            </div>
        </Card>
    );
}
