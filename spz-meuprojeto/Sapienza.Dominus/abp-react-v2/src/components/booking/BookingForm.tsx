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
  import { useCreateBooking, useUpdateBooking } from "@/lib/abp/hooks/useBookings";
import { toast } from "sonner";

const formSchema = z.object({
  
    clientName: z.any(),
  
    clientEmail: z.any(),
  
    startTime: z.any(),
  
    endTime: z.any(),
  
    status: z.any(),
  
    typeId: z.any(),
  
    clientId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface BookingFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function BookingForm({
  isOpen,
  onClose,
  initialValues,
}: BookingFormProps) {
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

  const createMutation = useCreateBooking();
const updateMutation = useUpdateBooking();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Booking updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Booking created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save booking:", error);
    toast.error(error.message || "Failed to save booking");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Booking": "Create Booking" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the booking." : "Fill in the details to create a new booking." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="clientName" > ClientName * </Label>

<Input id="clientName" {...register("clientName") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="clientEmail" > ClientEmail * </Label>

<Input id="clientEmail" {...register("clientEmail") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="startTime" > StartTime * </Label>

<Input id="startTime" type = "date" {...register("startTime") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="endTime" > EndTime * </Label>

<Input id="endTime" type = "date" {...register("endTime") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="status" > Status</Label>

<Input id="status" {...register("status") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="typeId" > TypeId</Label>

<Input id="typeId" {...register("typeId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="clientId" > ClientId</Label>

<Input id="clientId" {...register("clientId") } />

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
