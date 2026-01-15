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
  import { useCreateSiteVisitEvent, useUpdateSiteVisitEvent } from "@/lib/abp/hooks/useSiteVisitEvents";
import { toast } from "sonner";

const formSchema = z.object({
  
    visitorId: z.any(),
  
    siteId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface SiteVisitEventFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function SiteVisitEventForm({
  isOpen,
  onClose,
  initialValues,
}: SiteVisitEventFormProps) {
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

  const createMutation = useCreateSiteVisitEvent();
const updateMutation = useUpdateSiteVisitEvent();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("SiteVisitEvent updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("SiteVisitEvent created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save sitevisitevent:", error);
    toast.error(error.message || "Failed to save sitevisitevent");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit SiteVisitEvent": "Create SiteVisitEvent" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the sitevisitevent." : "Fill in the details to create a new sitevisitevent." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="visitorId" > VisitorId</Label>

<Input id="visitorId" {...register("visitorId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="siteId" > SiteId</Label>

<Input id="siteId" {...register("siteId") } />

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
