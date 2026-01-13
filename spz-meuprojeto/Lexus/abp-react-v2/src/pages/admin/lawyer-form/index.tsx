import { useEffect, useState } from 'react';
import { useLocation, useRoute } from 'wouter';
import { ChevronRight, Home } from 'lucide-react';
import { toast } from 'sonner';
import { Button } from '@/components/ui/button';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Shell } from '@/components/layout/shell';
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { GeneralTab } from './tabs/GeneralTab';
import { SpecializationsTab } from './tabs/SpecializationsTab';
import { useLawyer, useCreateLawyer, useUpdateLawyer } from '@/lib/abp/hooks/useLawyers';
import { useLawyerSpecializations, useToggleSpecialization } from '@/lib/abp/hooks/useLawyerSpecializations';
import { Skeleton } from '@/components/ui/skeleton';

export default function LawyerFormPage() {
    const [, navigate] = useLocation();
    const [match, params] = useRoute("/admin/lawyer/:id/edit");
    const id = match ? params?.id : undefined;
    const isEditMode = !!id;

    // State
    const [activeTab, setActiveTab] = useState('general');
    const [formData, setFormData] = useState({
        fullName: '',
        preferredName: '',
        concurrencyStamp: '',
    });
    const [selectedSpecializations, setSelectedSpecializations] = useState<Set<string>>(new Set());
    const [errors, setErrors] = useState<Record<string, string>>({});
    const [isDirty, setIsDirty] = useState(false);
    const [showUnsavedDialog, setShowUnsavedDialog] = useState(false);
    const [pendingNavigation, setPendingNavigation] = useState<string | null>(null);

    // Queries & Mutations
    const { data: lawyer, isLoading } = useLawyer(id || '');
    const { data: lawyerSpecs } = useLawyerSpecializations({ lawyerId: id });
    const createMutation = useCreateLawyer();
    const updateMutation = useUpdateLawyer();
    const toggleSpec = useToggleSpecialization(id || '');

    // Load data
    useEffect(() => {
        if (lawyer) {
            setFormData({
                fullName: lawyer.fullName || '',
                preferredName: lawyer.preferredName || '',
                concurrencyStamp: lawyer.concurrencyStamp || '',
            });
        }
    }, [lawyer]);

    useEffect(() => {
        if (lawyerSpecs?.items) {
            const specIds = new Set<string>(lawyerSpecs.items.map((ls: any) => ls.specializationId as string));
            setSelectedSpecializations(specIds);
        }
    }, [lawyerSpecs]);

    // Handle form changes
    const handleFieldChange = (field: string, value: string) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
        setIsDirty(true);
        if (errors[field]) {
            setErrors((prev) => {
                const newErrors = { ...prev };
                delete newErrors[field];
                return newErrors;
            });
        }
    };

    const handleSpecializationToggle = (specializationId: string, isChecked: boolean) => {
        setSelectedSpecializations((prev) => {
            const newSet = new Set(prev);
            if (isChecked) {
                newSet.add(specializationId);
            } else {
                newSet.delete(specializationId);
            }
            return newSet;
        });
        setIsDirty(true);
    };

    // Validation
    const validate = () => {
        const newErrors: Record<string, string> = {};
        if (!formData.fullName.trim()) {
            newErrors.fullName = 'Full name is required';
        }
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    // Save
    const handleSave = async () => {
        if (!validate()) {
            setActiveTab('general');
            toast.error('Please fill in all required fields');
            return;
        }

        try {
            let createdLawyerId = id;

            if (isEditMode) {
                // Update lawyer - use same pattern as Create (no ID in body)
                const updatePayload = {
                    fullName: formData.fullName,
                    preferredName: formData.preferredName,
                    concurrencyStamp: formData.concurrencyStamp
                };
                console.log('UPDATE PAYLOAD:', JSON.stringify(updatePayload, null, 2));
                console.log('UPDATE URL:', `/api/app/lawyer/${id}`);

                await updateMutation.mutateAsync({ id: id!, data: updatePayload });

                // Update specializations
                const currentSpecs = new Set(lawyerSpecs?.items?.map((ls: any) => ls.specializationId) || []);

                // Add new specializations
                for (const specId of selectedSpecializations) {
                    if (!currentSpecs.has(specId)) {
                        await toggleSpec.mutateAsync({ specializationId: specId, isChecked: false });
                    }
                }

                // Remove old specializations
                for (const specId of Array.from(currentSpecs) as string[]) {
                    if (!selectedSpecializations.has(specId)) {
                        await toggleSpec.mutateAsync({ specializationId: specId, isChecked: true });
                    }
                }

                toast.success('Lawyer updated successfully');
            } else {
                // Create lawyer - use camelCase (ABP .NET default)
                const createPayload = {
                    fullName: formData.fullName,
                    preferredName: formData.preferredName
                };
                console.log('CREATE PAYLOAD:', JSON.stringify(createPayload, null, 2));
                const result = await createMutation.mutateAsync(createPayload);
                createdLawyerId = result.id;

                // Add specializations
                for (const specId of selectedSpecializations) {
                    await toggleSpec.mutateAsync({ specializationId: specId, isChecked: false });
                }

                toast.success('Lawyer created successfully');
            }

            setIsDirty(false);
            navigate('/admin/lawyer');
        } catch (error: any) {
            console.error('Save failed:', error);
            console.error('Error response:', error.response?.data);
            console.error('Error status:', error.response?.status);
            toast.error('Failed to save lawyer. Please try again.');
        }
    };

    // Navigation with unsaved changes guard
    const handleNavigation = (path: string) => {
        if (isDirty) {
            setPendingNavigation(path);
            setShowUnsavedDialog(true);
        } else {
            navigate(path);
        }
    };

    const confirmNavigation = () => {
        if (pendingNavigation) {
            navigate(pendingNavigation);
        }
        setShowUnsavedDialog(false);
        setPendingNavigation(null);
    };

    const cancelNavigation = () => {
        setShowUnsavedDialog(false);
        setPendingNavigation(null);
    };

    const handleBack = () => handleNavigation('/admin/lawyer');
    const handleCancel = () => handleNavigation('/admin/lawyer');

    if (isLoading && isEditMode) {
        return (
            <Shell>
                <div className="space-y-6">
                    <Skeleton className="h-8 w-64 mb-4" />
                    <Skeleton className="h-10 w-full mb-4" />
                    <Skeleton className="h-96 w-full" />
                </div>
            </Shell>
        );
    }

    return (
        <Shell>
            <div className="space-y-6">
                {/* Breadcrumb */}
                <nav className="flex items-center space-x-2 text-sm text-muted-foreground">
                    <Home className="h-4 w-4" />
                    <ChevronRight className="h-4 w-4" />
                    <button
                        onClick={() => handleNavigation('/dashboard')}
                        className="hover:text-foreground transition-colors"
                    >
                        Dashboard
                    </button>
                    <ChevronRight className="h-4 w-4" />
                    <button
                        onClick={handleBack}
                        className="hover:text-foreground transition-colors"
                    >
                        Lawyers
                    </button>
                    <ChevronRight className="h-4 w-4" />
                    <span className="text-foreground font-medium">
                        {isEditMode ? 'Edit Lawyer' : 'Create Lawyer'}
                    </span>
                </nav>

                {/* Header */}
                <div className="flex items-center justify-between">
                    <div>
                        <Button
                            variant="ghost"
                            onClick={handleBack}
                            className="mb-2 -ml-4"
                        >
                            ← Back to Lawyers
                        </Button>
                        <h1 className="text-3xl font-bold">
                            {isEditMode ? 'Edit Lawyer' : 'Create Lawyer'}
                        </h1>
                    </div>
                </div>

                {/* Tabs */}
                <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-4">
                    <TabsList>
                        <TabsTrigger value="general">General</TabsTrigger>
                        <TabsTrigger value="specializations">Specializations</TabsTrigger>
                    </TabsList>

                    <TabsContent value="general" className="space-y-4">
                        <div className="flex justify-end space-x-2 mb-4">
                            <Button variant="outline" onClick={handleCancel}>
                                Cancel
                            </Button>
                            <Button onClick={handleSave} disabled={createMutation.isPending || updateMutation.isPending}>
                                {createMutation.isPending || updateMutation.isPending ? 'Saving...' : 'Save Changes'}
                            </Button>
                        </div>
                        <GeneralTab formData={formData} onChange={handleFieldChange} errors={errors} />
                    </TabsContent>

                    <TabsContent value="specializations" className="space-y-4">
                        <div className="flex justify-end space-x-2 mb-4">
                            <Button variant="outline" onClick={handleCancel}>
                                Cancel
                            </Button>
                            <Button onClick={handleSave} disabled={createMutation.isPending || updateMutation.isPending}>
                                {createMutation.isPending || updateMutation.isPending ? 'Saving...' : 'Save Changes'}
                            </Button>
                        </div>
                        <SpecializationsTab
                            lawyerId={id}
                            selectedSpecializations={selectedSpecializations}
                            onToggle={handleSpecializationToggle}
                        />
                    </TabsContent>
                </Tabs>
            </div>

            {/* Unsaved Changes Dialog */}
            <AlertDialog open={showUnsavedDialog} onOpenChange={setShowUnsavedDialog}>
                <AlertDialogContent>
                    <AlertDialogHeader>
                        <AlertDialogTitle>Unsaved Changes</AlertDialogTitle>
                        <AlertDialogDescription>
                            You have unsaved changes. Are you sure you want to leave? Your changes will be lost.
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                        <AlertDialogCancel onClick={cancelNavigation}>Stay on Page</AlertDialogCancel>
                        <AlertDialogAction onClick={confirmNavigation}>Leave Anyway</AlertDialogAction>
                    </AlertDialogFooter>
                </AlertDialogContent>
            </AlertDialog>
        </Shell>
    );
}
