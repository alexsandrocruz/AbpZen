import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetTimeEntriesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  projectId?: string;
  }

export function useTimeEntries(input: GetTimeEntriesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["timeEntries", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/time-entry", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllTimeEntries() {
  return useQuery({
    queryKey: ["timeEntries", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/time-entry", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useTimeEntry(id: string) {
  return useQuery({
    queryKey: ["timeEntry", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/time-entry/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateTimeEntry() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/time-entry", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["timeEntries"] });
    },
  });
}

export function useUpdateTimeEntry() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/time-entry/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["timeEntries"] });
      queryClient.invalidateQueries({ queryKey: ["timeEntry", data.id] });
    },
  });
}

export function useDeleteTimeEntry() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/time-entry/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["timeEntries"] });
    },
  });
}


