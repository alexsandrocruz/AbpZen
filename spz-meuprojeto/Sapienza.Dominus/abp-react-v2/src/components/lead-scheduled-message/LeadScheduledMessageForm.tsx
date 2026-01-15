import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateLeadScheduledMessage, useUpdateLeadScheduledMessage } from "@/lib/abp/hooks/useLeadScheduledMessages";
import { toast } from "sonner";

const formSchema = z.object({
  
    scheduledFor: z.any(),
  
    status: z.any(),
  
    automationId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface LeadScheduledMessageFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function LeadScheduledMessageForm({
  isOpen,
  onClose,
  initialValues,
}: LeadScheduledMessageFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateLeadScheduledMessage();
const updateMutation = useUpdateLeadScheduledMessage();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("LeadScheduledMessage updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("LeadScheduledMessage created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save leadscheduledmessage:", error);
    toast.error(error.message || "Failed to save leadscheduledmessage");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit LeadScheduledMessage": "Create LeadScheduledMessage" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the leadscheduledmessage." : "Fill in the details to create a new leadscheduledmessage." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="scheduledFor" > ScheduledFor * </Label>

<Input id="scheduledFor" type = "date" {...register("scheduledFor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="status" > Status * </Label>

<Input id="status" {...register("status") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="automationId" > AutomationId</Label>

<Input id="automationId" {...register("automationId") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
