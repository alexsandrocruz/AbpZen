import { Shell } from "@/components/layout/shell";
import { EventForm } from "@/components/event/EventForm";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { useLocation, useParams } from "wouter";
import { useEvents } from "@/lib/abp/hooks/useEvents";
import { Loader2, ArrowLeft } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function EventFormPage() {
    const [, setLocation] = useLocation();
    const { id } = useParams();
    const isEditing = !!id;

    const { data: eventData, isLoading } = useEvents(
        {}, // Query params
        { enabled: isEditing } // Only fetch if editing
    );

    const initialValues = isEditing
        ? eventData?.items?.find((item: any) => item.id === id)
        : null;

    if (isEditing && isLoading) {
        return (
            <Shell>
                <div className="flex items-center justify-center h-[50vh]">
                    <Loader2 className="h-8 w-8 animate-spin text-primary" />
                </div>
            </Shell>
        );
    }

    return (
        <Shell>
            <div className="max-w-4xl mx-auto space-y-6">
                <div className="flex items-center gap-4">
                    <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => setLocation("/admin/event")}
                    >
                        <ArrowLeft className="h-4 w-4" />
                    </Button>
                    <h1 className="text-3xl font-bold tracking-tight">
                        {isEditing ? "Edit Event" : "New Event"}
                    </h1>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                    <div className="lg:col-span-2">
                        <Card>
                            <CardHeader>
                                <CardTitle>Event Details</CardTitle>
                                <CardDescription>
                                    Enter the core information for this event.
                                </CardDescription>
                            </CardHeader>
                            <CardContent>
                                <EventForm
                                    isStandalone
                                    initialValues={initialValues}
                                    onClose={() => setLocation("/admin/event")}
                                />
                            </CardContent>
                        </Card>
                    </div>

                    <div className="space-y-6">
                        <Card>
                            <CardHeader>
                                <CardTitle>Sidebar Info</CardTitle>
                                <CardDescription>
                                    Useful context for the event creation.
                                </CardDescription>
                            </CardHeader>
                            <CardContent className="text-sm text-muted-foreground">
                                <p>Ensure all required fields marked with * are filled.</p>
                                <div className="mt-4 p-3 bg-secondary rounded-md">
                                    <strong>Tip:</strong> You can check for artist conflicts in real-time.
                                </div>
                            </CardContent>
                        </Card>
                    </div>
                </div>
            </div>
        </Shell>
    );
}
