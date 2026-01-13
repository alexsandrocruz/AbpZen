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
  import { useCreateAvailability, useUpdateAvailability } from "@/lib/abp/hooks/useAvailabilities";
import { toast } from "sonner";

const formSchema = z.object({
  
    type: z.any(),
  
    artistId: z.any(),
  
    startDate: z.any(),
  
    endDate: z.any(),
  
    notes: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AvailabilityFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AvailabilityForm({
  isOpen,
  onClose,
  initialValues,
}: AvailabilityFormProps) {
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

  const createMutation = useCreateAvailability();
const updateMutation = useUpdateAvailability();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Availability updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Availability created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save availability:", error);
    toast.error(error.message || "Failed to save availability");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Availability": "Create Availability" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the availability." : "Fill in the details to create a new availability." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="type" > Type * </Label>

<Input id="type" {...register("type") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="artistId" > ArtistId</Label>

<Input id="artistId" {...register("artistId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="startDate" > StartDate</Label>

<Input id="startDate" type = "date" {...register("startDate") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="endDate" > EndDate</Label>

<Input id="endDate" type = "date" {...register("endDate") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="notes" > Notes</Label>

<Input id="notes" {...register("notes") } />

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
